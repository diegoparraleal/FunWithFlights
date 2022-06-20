import {createContext} from "react";
import {InitialState, Reducer} from "./reducers";
import {useReducerWithMiddleware} from "../hooks";
import {Middleware} from "./middleware";

export const Store = createContext(null);
export function StoreProvider({children}){
    const [state, dispatch] = useReducerWithMiddleware(Reducer, InitialState, Middleware);
    return (<Store.Provider value={{state, dispatch}}>{children}</Store.Provider>)
} 