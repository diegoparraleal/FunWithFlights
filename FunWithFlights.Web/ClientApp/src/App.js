import React from 'react';
import { Route } from 'react-router';
import { Layout } from './components';

import './custom.css'
import {StoreProvider} from "./store";
import {Routes} from "./pages";

export default function App() {
    return (
        <StoreProvider>
            <Layout>
                <Route exact path='/' component={Routes} />
                <Route path='/home' component={Routes} />
            </Layout>      
        </StoreProvider>
    );
}
