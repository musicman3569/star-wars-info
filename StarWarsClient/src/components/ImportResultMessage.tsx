import React from 'react';
import type {ImportResult} from "../utils/StarWarsInfoClient.ts";

interface ImportResultMessageProps {
    importResult: ImportResult;
}

export const ImportResultMessage: React.FC<ImportResultMessageProps> = ({importResult}) => {
    return (
        <div className="flex flex-column gap-2">
            <div
                className={`text-lg font-bold ${importResult.status === 'complete' ? 'text-green-500' : 'text-red-500'}`}>
                Status: {importResult.status}
            </div>
            <div className="text-lg">
                {importResult.message}
            </div>
            <div className="flex flex-column gap-1 mt-2">
                <div className="font-medium">Import Statistics:</div>
                <div>Starships: {importResult.starship_import_count}</div>
                <div>Films: {importResult.film_import_count}</div>
                <div>Planets: {importResult.planet_import_count}</div>
                <div>People: {importResult.people_import_count}</div>
                <div>Species: {importResult.species_import_count}</div>
                <div>Vehicles: {importResult.vehicle_import_count}</div>
            </div>
            <div className="mt-2">
                <span className="font-medium">Refresh table now? </span>
            </div>
        </div>
    );
};