import {type ModelSpec} from '../utils/DataTableColumn';

const ModelPerson: ModelSpec = {
    name: {kind: 'text', frozen: true},
    height: {kind: 'number'},
    mass: {kind: 'number'},
    hair_color: {kind: 'text'},
    skin_color: {kind: 'text'},
    eye_color: {kind: 'text'},
    birth_year: {kind: 'text'},
    gender: {kind: 'text'},
    homeworld_id: {kind: 'number'},
    person_id: {kind: 'id', width: '12rem', isDataKey: true, isHidden: false, isReadOnly: true},
    created: {kind: 'date', isReadOnly: true},
    edited: {kind: 'date', isReadOnly: true}
};

export default ModelPerson;