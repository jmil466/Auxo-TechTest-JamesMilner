import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import OrderScreen from './components/OrderScreen';

const AppRoutes = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<OrderScreen />} />
      </Routes>
    </Router>
  );
};

export default AppRoutes;