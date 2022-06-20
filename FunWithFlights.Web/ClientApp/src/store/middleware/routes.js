import {ACTIONS, Actions} from "../actions";
import {routeService} from "../../services";

async function searchRoutes(dispatch, payload){
    try{
        dispatch(Actions.RouteActions.setError(false));
        dispatch(Actions.RouteActions.setLoading(true));
        const data = await routeService.searchRoutes(payload);
        dispatch(Actions.RouteActions.setResults(data));
    } catch (e) {
        console.error('Error searchRoutes: ', e);
        dispatch(Actions.RouteActions.setError(true));
    } finally {
        dispatch(Actions.RouteActions.setLoading(false));
    }
}

export const RouteMiddleware = (dispatch, action) => {
    switch (action.type) {
        case ACTIONS.ROUTES.SEARCH: return searchRoutes(dispatch, action.payload);
        default: return dispatch;
    }
}