export const ROUTE_ACTIONS = {    
    SEARCH: 'routes.search',
    SET_RESULTS: 'routes.setResults',
    SET_ERROR: 'routes.setError',
    SET_LOADING: 'routes.setLoading'
};

export const RouteActions = {
    search: (payload) => ({type: ROUTE_ACTIONS.SEARCH, payload}),
    setResults: (results) => ({type: ROUTE_ACTIONS.SET_RESULTS, payload: results}),
    setError: (error) => ({type: ROUTE_ACTIONS.SET_ERROR, payload: error}),
    setLoading: (loading) => ({type: ROUTE_ACTIONS.SET_LOADING, payload: loading})
};