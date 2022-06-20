import {RouteInitialState, RouteReducer} from "./routes";

export const InitialState = {
    route: RouteInitialState
};

export const Reducer = (state = InitialState, action) => {
    return {
        ...state,
        route: RouteReducer(state.route, action)
    };
};