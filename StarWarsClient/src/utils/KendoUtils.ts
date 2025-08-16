import {convertStringDates} from "./DateUtils.ts";

/**
 * Fetches data from the Star Wars API and processes date fields.
 *
 * @param swapiModel - The type of Star Wars data to fetch ('characters', 'films', 'planets', 'species', 'starships', 'vehicles')
 * @param dateFields - Array of field names that should be converted from string to Date objects
 * @param useStateCallback - React setState callback function to update the component's state with the fetched data
 *
 * @example
 * // Fetch starships data and convert 'created' and 'edited' fields to Date objects
 * const [starships, setStarships] = useState<any[]>([]);
 * FetchData('starships', ['created', 'edited'], setStarships);
 *
 * @throws {Error} Logs error message to console if the fetch operation fails
 */
export function FetchData(
  swapiModel: 'characters' | 'films' | 'planets' | 'species' | 'starships' | 'vehicles',
  dateFields: string[],
  useStateCallback: (data: any[]) => void
) {
  fetch('https://api.starwarsinfo.test:8080/' + swapiModel + '/getall')
    .then(response => response.json())
    .then(data => {
      useStateCallback(
        convertStringDates(data, dateFields)
      );
    })
    .catch(error => console.log('Error fetching '+ swapiModel +' data: ', error));
}

