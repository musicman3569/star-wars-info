import {
    type FilterSpec
} from '../utils/DataTableFilters.tsx';

export interface StarshipModel {
    starship_id: number;
    name: string;
    model: string;
    manufacturer: string;
    cost_in_credits: number;
    length: number;
    max_atmosphering_speed: number;
    crew: number;
    passengers: number;
    cargo_capacity: number;
    consumables: string;
    hyperdrive_rating: number;
    MGLT: number;
    starship_class: string;
    created: Date;
    edited: Date;
}

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
