import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import Games from './components/Games';
import Authenticate from './components/auth/Authenticate';
import { createBrowserRouter, RouterProvider } from "react-router-dom"

import { Provider } from 'react-redux';
import { store } from './redux/store';

const router = createBrowserRouter([
 { path: "/", element: <App /> },
 { path: "/Games", element: <Games /> },
 { path: "/Authenticate", element: <Authenticate />}
]);

const rootElement = document.getElementById("root");
if(!rootElement) throw new Error("Failed to find the root element.");
const root = ReactDOM.createRoot(rootElement);

root.render(
  <React.StrictMode>
    <Provider store={store}>
      <RouterProvider router={router} />
    </Provider>
  </React.StrictMode>
)

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
//reportWebVitals();
