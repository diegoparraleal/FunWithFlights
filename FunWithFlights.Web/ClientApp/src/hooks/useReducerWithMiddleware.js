import {useCallback, useReducer} from "react";

export  const useReducerWithMiddleware = (reducer, initialState, middleware) => {
    const [state, dispatch] = useReducer(reducer, initialState);
    const dispatchUsingMiddleware = useCallback( (action) => {
        middleware(dispatch, action);
        dispatch(action);
    }, [ middleware, dispatch ]);
    return [state, dispatchUsingMiddleware];
}; 