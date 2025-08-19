import { convertStringDates } from "./DateUtils";
import { type ModelSpec } from "./DataTableColumn";

/**
 * Fetches data from the Star Wars API and processes date fields.
 *
 * @param modelSpec - The model specification containing field definitions
 * @param modelDataKey - The key field name in the model (usually ends with '_id')
 * @param useStateCallback - React setState callback function to update the component's state with the fetched data
 *
 * @example
 * // Fetch "starships" data and convert 'created' and 'edited' fields to Date objects
 * const [starships, setStarships] = useState<any[]>([]);
 * FetchData('starships', ['created', 'edited'], setStarships);
 *
 * @throws {Error} Logs an error message to console if the fetch operation fails
 */
export function FetchData(
    modelSpec: ModelSpec,
    modelDataKey: string,
    successCallback: (data: any[]) => void
) {
    const apiUrl = getApiUrl(modelDataKey);

    fetch(apiUrl.fullUrl)
        .then(response => response.json())
        .then(data => {
            successCallback(
                convertStringDates(data, getDateFields(modelSpec))
            );
        })
        .catch(error => console.log('Error fetching '+ apiUrl.path +' data: ', error));
}

export function UpdateData(
    modelSpec: ModelSpec,
    modelDataKey: string,
    newData: any,
    successCallback: (responseData: any) => void,
) {
    const apiUrl = getApiUrl(modelDataKey);

    fetch(apiUrl.fullUrl, {
        method: 'PUT',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(newData)
    })
        .then(response => response.json())
        .then(updatedModel => {
            successCallback(
                convertStringDates([updatedModel], getDateFields(modelSpec)).at(0) as any
            );
        })
        .catch(error => console.log('Error updating ' + apiUrl.path + ' data: ', error));
}

function getApiUrl(modelDataKey: string) {
    const apiUrl = import.meta.env.VITE_API_URL;
    const apiPath = modelDataKey.replace('_id', '');
    return {
        fullUrl: apiUrl + '/' + apiPath,
        path: apiPath,
    };
}

function getDateFields(modelSpec: ModelSpec) {
    return Object.keys(modelSpec).filter(field =>
        modelSpec[field].dataType === 'date' ||
        modelSpec[field].kind === 'date'
    );
}