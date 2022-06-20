import React, {useCallback, useContext, useState} from 'react';
import {Actions, Store} from "../../store";
import {Button, Input, InputGroup} from "reactstrap";
import {RouteTable} from "./RouteTable";

import './Routes.css';

export function Routes() {
  const context = useContext(Store);
  const dispatch = context.dispatch;
  const state = context.state;
  const results = state.route.results;
  const loading = state.route.loading;
  const error = state.route.error;
  const [from, setFrom] = useState("");
  const [to, setTo] = useState("");

  const handleOnChangeFrom = useCallback((ev) => setFrom(ev.target.value), [ setFrom ]);
  const handleOnChangeTo = useCallback((ev) => setTo(ev.target.value), [ setTo ]);
  const handleSearch = useCallback(() => {
    dispatch(Actions.RouteActions.search({from, to}));
  }, [ from, to, dispatch]);

  const errorImage = "https://seupdateblog.files.wordpress.com/2019/01/server-error-what-is-5xx-http-status-error.jpg";
  const welcomeImage = "https://www.airlive.net/wp-content/uploads/2021/06/airport-3511342_1280.jpg";
  const contents = loading
    ? <p><em>Loading...</em></p>
    : results === null 
          ? <img 
              className="filler-image" 
              alt={error ? "error": "welcome"} 
              src={error ? errorImage: welcomeImage}
            />
          : <RouteTable results={results} />;
  
  return (
    <div>
      <h5 id="tableLabel" >Search routes</h5>
      <InputGroup>
        <Input value={from} onChange={handleOnChangeFrom} placeholder="From ..." />
        <Input value={to} onChange={handleOnChangeTo} placeholder="To ..." />
        <Button onClick={handleSearch}>Search</Button>
      </InputGroup>
      {contents}
    </div>
  );
}
