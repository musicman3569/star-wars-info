import {
    type FilterSpec
} from '../utils/DataTableFilters.tsx';

export const StarshipModelFilterSpec: FilterSpec = {
    starship_id: {kind: 'id'},
    name: {kind: 'text', frozen: true},
    model: {kind: 'text'},
    manufacturer: {kind: 'text'},
    cost_in_credits: {kind: 'number'},
    length: {kind: 'number', decimalPlaces: 2},
    max_atmosphering_speed: {kind: 'number'},
    crew: {kind: 'number'},
    passengers: {kind: 'number'},
    cargo_capacity: {kind: 'number'},
    consumables: {kind: 'text'},
    hyperdrive_rating: {kind: 'number', decimalPlaces: 1},
    MGLT: {kind: 'number'},
    starship_class: {kind: 'text'},
    created: {kind: 'date'},
    edited: {kind: 'date'}
};
