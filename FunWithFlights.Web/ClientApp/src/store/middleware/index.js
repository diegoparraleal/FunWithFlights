import {RouteMiddleware} from "./routes";

export const Middleware = async (dispatch, action) => {
    await RouteMiddleware(dispatch, action);
    return dispatch;
} 