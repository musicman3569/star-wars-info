import {type ModelSpec} from '../utils/DataTableColumn';

const ModelPlanet: ModelSpec = {
    name: {kind: 'text', frozen: true},
    rotation_period: {kind: 'number'},
    orbital_period: {kind: 'number'},
    diameter: {kind: 'number'},
    climate: {kind: 'text'},
    gravity: {kind: 'number', decimalPlaces: 2, displaySuffix: ' standard'},
    terrain: {kind: 'text'},
    surface_water: {kind: 'number'},
    population: {kind: 'number'},
    planet_id: {kind: 'id', width: '12rem', isDataKey: true, isHidden: false, isReadOnly: true},
    created: {kind: 'date', isReadOnly: true},
    edited: {kind: 'date', isReadOnly: true}
};

export default ModelPlanet;