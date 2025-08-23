import { type ModelSpec } from '../utils/DataTableColumn';

const ModelFilm: ModelSpec = {
    film_id: {kind: 'id', width: '12rem', isDataKey: true, isHidden: false, isReadOnly: true},
    title: {kind: 'text'},
    episode_id: {kind: 'number'},
    opening_crawl: {kind: 'text'},
    director: {kind: 'text'},
    producer: {kind: 'text'},
    release_date: {kind: 'date'},
    created: {kind: 'date', isReadOnly: true},
    edited: {kind: 'date', isReadOnly: true}
};

export default ModelFilm;