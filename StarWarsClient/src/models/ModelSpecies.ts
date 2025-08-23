import {type ModelSpec} from '../utils/DataTableColumn';

const ModelSpecies: ModelSpec = {
    name: {kind: 'text', frozen: true},
    classification: {kind: 'text'},
    designation: {kind: 'text'},
    average_height: {kind: 'number'},
    skin_colors: {kind: 'text'},
    hair_colors: {kind: 'text'},
    eye_colors: {kind: 'text'},
    average_lifespan: {kind: 'number'},
    homeworld_id: {kind: 'number'},
    language: {kind: 'text'},
    species_id: {kind: 'id', width: '12rem', isDataKey: true, isHidden: false, isReadOnly: true},
    created: {kind: 'date', isReadOnly: true},
    edited: {kind: 'date', isReadOnly: true}
};

export default ModelSpecies;