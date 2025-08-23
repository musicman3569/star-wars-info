import React, {useState, useEffect, useMemo, useRef} from 'react';
import {DeleteData, FetchData, ImportData, UpdateData} from '../utils/StarWarsInfoClient';
import {DataTable, type DataTableFilterMeta} from 'primereact/datatable';
import SwapiColumn from './SwapiColumn';
import {type ModelSpec, getModelDataKey, buildDefaultFilters} from '../utils/DataTableColumn';
import {Column} from "primereact/column";
import {useCachedFilterCallbacks} from "../utils/DataTableFilterCache.ts";
import DataTableHeader from "./DataTableHeader.tsx";
import DataTableEditForm from "./DataTableEditForm.tsx";
import {Button} from "primereact/button";
import {ConfirmDialog, confirmDialog} from "primereact/confirmdialog";
import {useKeycloak} from "@react-keycloak/web";
import { Toast } from 'primereact/toast';

/**
 * A data table component for displaying and managing Star Wars information.
 * Supports filtering, editing, adding, and deleting rows of data.
 * @param {Object} props - Component props
 * @param {ModelSpec} props.modelSpec - Specification of the data model structure
 */
function SwapiDataTable({
    modelSpec
}:{
    modelSpec: ModelSpec;
}) {
    const cssHeightToPageBottom = "calc(100vh - 100px - 50px)";
    const modelDataKey = getModelDataKey(modelSpec);
    const { keycloak, initialized } = useKeycloak();
    const toast = useRef<Toast>(null);
    const [windowWidth, setWindowWidth] = useState(window.innerWidth);

    const defaultFilters = useMemo(() => buildDefaultFilters(modelSpec), [modelSpec]);
    const filterCallbacks = useCachedFilterCallbacks();
    const [tableData, setTableData] = useState<any[]>([]);
    const [filters, setFilters] = useState<DataTableFilterMeta>(defaultFilters);
    const [globalFilterValue, setGlobalFilterValue] = useState('');
    const [editFormVisible, setEditFormVisible] = useState(false);
    const [loading, setLoading] = useState(true);

    /**
     * Clears the global filter and resets filters to their default values
     */
    const clearGlobalFilter = () => {
        setFilters(defaultFilters);
        setGlobalFilterValue('');
    };

    /**
     * Handles changes to the global filter input
     * @param {React.ChangeEvent<HTMLInputElement>} e - Input change event
     */
    const onGlobalFilterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setGlobalFilterValue(value);
        setFilters((prev) => {
            const next = { ...prev };
            // @ts-ignore – PrimeReact types don't index cleanly
            next['global'].value = value;
            return next;
        });
    };

    /**
     * Effect hook that updates the window width when the component mounts or when the window resizes.
     * This is used for responsive layout such as unfreezing columns when the screen is narrow to make
     * more readable space on the table.
     */
    useEffect(() => {
        const handleResize = () => setWindowWidth(window.innerWidth);
        window.addEventListener('resize', handleResize);
        return () => window.removeEventListener('resize', handleResize);
    }, []);
    

    /**
     * Effect hook that fetches data when the component mounts or when dependencies change.
     * Waits for Keycloak initialization and valid token before making the API call.
     * Updates the table data and loading state.
     *
     * @dependency {boolean} initialized - Keycloak initialization status
     * @dependency {string} keycloak.token - Authentication token
     * @dependency {ModelSpec} modelSpec - Data model specification
     * @dependency {string} modelDataKey - Key field for the data model
     */
    useEffect(() => {
        if (!initialized) return;
        
        FetchData(
            modelSpec, 
            modelDataKey,
            (data) => {
                setTableData(data);
                if (data.length === 0) {
                    confirmDialog({
                        message: `No database records were found. Would you like to import data from the Star Wars API now?`,
                        header: `Import Data`,
                        icon: 'pi pi-cloud-download',
                        defaultFocus: 'accept',
                        accept: importData,
                    });
                }
            },
            keycloak.token
        ).then();
        
        setLoading(false);
    }, [initialized, keycloak.token, modelSpec, modelDataKey]);
    
    const importData = () => {
        if (!initialized) return;
        setLoading(true);
        toast.current?.show({
            severity: 'info',
            summary: 'Data Import',
            detail: "Importing data from SWAPI to database...",
            life: 3000
        });
        ImportData((importResult) => {
            confirmDialog({
                message: `Imported ${importResult.starship_import_count} starships, ${importResult.film_import_count} films. ${importResult.message} | Reload table now?`,
                header: `Import Data Complete`,
                icon: 'pi pi-cloud-download',
                defaultFocus: 'accept',
                accept: () => {
                    toast.current?.show({
                        severity: 'info',
                        summary: 'Loading Data',
                        detail: "Refreshing table data, please wait...",
                        life: 3000
                    });
                    FetchData(
                        modelSpec,
                        modelDataKey,
                        (data) => {
                            setTableData(data);
                        },
                        keycloak.token
                    ).then();
                },
            });},
            keycloak.token
        );
        setLoading(false);
    }

    /**
     * Handles the completion of row editing by updating the data
     * @param {any} newRowData - The updated row data
     */
    const onRowEditComplete = (newRowData:any) => {
        if (!initialized) return;
        UpdateData(
            modelSpec, 
            modelDataKey,
            newRowData,
            (responseData) => {
                const newTableData = [...tableData];
                const updatedRowIndex = newTableData.findIndex(item => 
                    item[modelDataKey] === responseData[modelDataKey]
                );
                
                // -1 means the row doesn't exist in the table, so we push it on the end
                if (updatedRowIndex === -1) {
                    newTableData.push(responseData);   
                } else {
                    newTableData[updatedRowIndex] = responseData;
                }
                
                setTableData(newTableData);
                toast.current?.show({
                    severity: 'success',
                    summary: 'Update Success',
                    detail: `${responseData.name} updated successfully`,
                    life: 3000
                });
            },
            keycloak.token
        );
    }

    /**
     * Shows the edit form for adding a new row
     */
    const onClickRowAdd = () => {
        if (!editFormVisible) {
            setEditFormVisible(true);
        }
    }

    /**
     * Deletes a row from the table
     * @param {any} rowData - The data of the row to be deleted
     */
    const onClickRowDelete = (rowData: any) => {
        if (!initialized) return;
        DeleteData(
            modelDataKey, 
            rowData[modelDataKey],
            () => {
                const newTableData = [...tableData];
                const deletedRowIndex = newTableData.findIndex(item =>
                    item[modelDataKey] === rowData[modelDataKey]
                );
                newTableData.splice(deletedRowIndex, 1);
                setTableData(newTableData);
                toast.current?.show({
                    severity: 'success',
                    summary: 'Delete Success',
                    detail: `${rowData.name} deleted successfully`,
                    life: 3000
                });
            },
            keycloak.token
        );
    }

    return (<>
        <DataTable
            value={tableData}
            header={DataTableHeader({
                globalFilterValue,
                onGlobalFilterChange,
                clearGlobalFilter,
                onClickRowAdd
            })}
            paginator={false}
            rows={10}
            rowHover={true}
            scrollable={true}
            scrollHeight={loading ? "0px" : cssHeightToPageBottom}
            dataKey={modelDataKey}
            sortMode="single"
            sortField="name"
            filters={filters}
            onFilter={(e) => setFilters(e.filters as DataTableFilterMeta)}
            globalFilterFields={[modelDataKey]}
            removableSort
            editMode="row"
            onRowEditComplete={(e) => onRowEditComplete(e.newData)}
            loading={loading}
            emptyMessage=" "
        >
            <Column rowEditor header="Edit" frozen style={{width: '1rem'}} />
            {
                Object.entries(modelSpec)
                    .map(([field, spec]) => 
                        SwapiColumn({
                            field: field, 
                            spec: spec,
                            filterCallbacks: filterCallbacks,
                            windowWidth: windowWidth
                        })
                    )
            }
            <Column 
                header="Delete" 
                body={(rowData) => 
                    <Button 
                        icon="pi pi-trash" 
                        onClick={() => {
                            confirmDialog({
                                message: `Are you sure you want to delete ${rowData.name}?`,
                                header: `Delete Row`,
                                icon: 'pi pi-exclamation-triangle',
                                defaultFocus: 'cancel',
                                accept: () => onClickRowDelete(rowData),
                            });
                        }}
                    />
                }   
            />
        </DataTable>
        <DataTableEditForm 
            visible={editFormVisible}
            onHide={() => setEditFormVisible(false)}
            modelSpec={modelSpec}
            onSave={(formData) => onRowEditComplete(formData)}
        />
        <ConfirmDialog />
        <Toast ref={toast} />
    </>);
}

export default SwapiDataTable;