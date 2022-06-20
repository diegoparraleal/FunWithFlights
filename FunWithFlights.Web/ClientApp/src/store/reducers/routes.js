import {ACTIONS} from "../actions";

export const RouteInitialState = {
    results: null,
    error: false,
    loading: false
};

export const RouteReducer = (state = RouteInitialState, {type, payload}) => {
    switch (type) {
        case ACTIONS.ROUTES.SET_RESULTS: return {...state, results: payload.results};
        case ACTIONS.ROUTES.SET_ERROR: return {...state, error: payload};
        case ACTIONS.ROUTES.SET_LOADING: return {...state, loading: payload};
        default: return state;
    }
};