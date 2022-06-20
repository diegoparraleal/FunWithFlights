import React, {useContext} from "react";
import './RouteTable.css';

export function RouteTable({results}){
    if (results.length === 0) 
        return (<img alt="not found" 
                     className="not-found"
                     src="https://cdn.dribbble.com/users/1135689/screenshots/3957784/media/2957ab45daa7143409a927df0cb2e87a.png"
                />);

    return (
        <ul className="route-table">
            {results.map((compositeRoute, id) =>
                <li key={id} className="composite-route-item">
                    <span>
                        <h3 className="composite-route-description">{compositeRoute.source} - {compositeRoute.destination}</h3>
                        <h5 className="composite-route-num-stops">{compositeRoute.numStops} Stops</h5>
                        <h6 className="composite-route-distance">{compositeRoute.distance} kms</h6>
                    </span>
                    <ul>
                        {compositeRoute.routes.map((route, id) =>
                            <li key={id} className="route-item">
                                <span>
                                    <h4 className="title">FROM:</h4>
                                    <h4>{route.source.name}</h4>
                                </span>
                                <span>
                                    <h4 className="title">TO:</h4>
                                    <h4>{route.destination.name}</h4>
                                </span>
                                <h6>Operated by {route.airline.name}</h6>
                            </li>
                        )}
                    </ul>
                </li>
            )}
        </ul>
    );
}