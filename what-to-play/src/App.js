import logo from './logo.svg';
import './App.css';
import React from 'react';
import Home from './components/Home';
import { Routes, Route } from 'react-router-dom';

export function App() {
  return (
    <div>
      <Home />
    </div>
  );
}

export default App;