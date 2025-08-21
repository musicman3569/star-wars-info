import { type ModelSpec } from '../utils/DataTableColumn';

const ModelStarship: ModelSpec = {
    name: {kind: 'text', frozen: true},
    model: {kind: 'text'},
    manufacturer: {kind: 'text'},
    cost_in_credits: {kind: 'number'},
    length: {kind: 'number', decimalPlaces: 2},
    max_atmosphering_speed: {kind: 'number', width: '18rem', displaySuffix: ' km'},
    crew: {kind: 'number'},
    passengers: {kind: 'number'},
    cargo_capacity: {kind: 'number'},
    consumables: {kind: 'text'},
    hyperdrive_rating: {kind: 'number', decimalPlaces: 1},
    MGLT: {kind: 'number'},
    starship_class: {kind: 'text'},
    starship_id: {kind: 'id', width: '12rem', isDataKey: true, isHidden: false, isReadOnly: true},
    created: {kind: 'date', isReadOnly: true},
    edited: {kind: 'date', isReadOnly: true}
};

export default ModelStarship;