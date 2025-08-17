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
    starship_id: {kind: 'number'},
    name: {kind: 'text'},
    model: {kind: 'text'},
    manufacturer: {kind: 'text'},
    cost_in_credits: {kind: 'between'},
    length: {kind: 'between'},
    max_atmosphering_speed: {kind: 'between'},
    crew: {kind: 'between'},
    passengers: {kind: 'between'},
    cargo_capacity: {kind: 'between'},
    consumables: {kind: 'text'},
    hyperdrive_rating: {kind: 'between'},
    MGLT: {kind: 'between'},
    starship_class: {kind: 'text'},
    created: {kind: 'date'},
    edited: {kind: 'date'}
};
