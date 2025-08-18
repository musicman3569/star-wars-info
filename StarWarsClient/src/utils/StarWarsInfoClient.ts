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
    useStateCallback: (data: any[]) => void
) {
    const apiUrl = import.meta.env.VITE_API_URL;
    const apiPath = modelDataKey.replace('_id', '');
    const dateFields = Object.keys(modelSpec).filter(field => 
        modelSpec[field].dataType === 'date' ||
        modelSpec[field].kind === 'date'
    );

    fetch(apiUrl + '/' + apiPath + '/getall')
        .then(response => response.json())
        .then(data => {
            useStateCallback(
                convertStringDates(data, dateFields)
            );
        })
        .catch(error => console.log('Error fetching '+ apiPath +' data: ', error));
}

