import { type ModelSpec } from '../utils/DataTableColumn';

const ModelStarship: ModelSpec = {
    starship_id: {kind: 'id', frozen: true, width: '10rem', isDataKey: true},
    name: {kind: 'text', frozen: true},
    model: {kind: 'text'},
    manufacturer: {kind: 'text'},
    cost_in_credits: {kind: 'number'},
    length: {kind: 'number', decimalPlaces: 2},
    max_atmosphering_speed: {kind: 'number', width: '18rem'},
    crew: {kind: 'number'},
    passengers: {kind: 'number'},
    cargo_capacity: {kind: 'number'},
    consumables: {kind: 'text'},
    hyperdrive_rating: {kind: 'number', decimalPlaces: 1},
    MGLT: {kind: 'number'},
    starship_class: {kind: 'text'},
    created: {kind: 'date', isReadOnly: true},
    edited: {kind: 'date', isReadOnly: true}
};

export default ModelStarship;