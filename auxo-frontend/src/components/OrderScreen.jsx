import React, { useState, useEffect } from 'react';
import { GetParts, PlaceOrder } from '../api/PartsService';
import './OrderScreen.css';

function OrderScreen() {

  const [parts, setParts] = useState([]);
  const [orderLines, setOrderLines] = useState([]);
  const [selectedPart, setSelectedPart] = useState('');
  const [quantity, setQuantity] = useState(0);
  const [error, setError] = useState('');
  const [placedOrders, setPlacedOrders] = useState([]);

  // Calculate total order cost
  const totalOrderCost = orderLines.reduce((total, item) => {
    const part = parts.find((part) => part.id === parseInt(item.PartId, 10));
    const itemCost = part ? part.price * item.OrderQuantity : 0;
    return total + itemCost;
  }, 0);

  // Calling GetData
  const fetchData = async () => {
    try {
      const partsData = await GetParts();
      setParts(partsData);
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };

  // Call Data on PageLoad
  useEffect(() => {
    fetchData();
  }, []);

  // Function to add the part to the order
  const addToOrder = () => {
    const orderLine = { PartId: selectedPart.id, OrderQuantity: quantity };
    console.log(orderLine);
    setOrderLines((prevOrderLines) => [...prevOrderLines, orderLine]);

    var availableUnits = selectedPart.quantity - quantity;
    console.log(availableUnits);

    // Stops people adding more to the order than is available
    setSelectedPart((prevSelectedPart) => ({
      ...prevSelectedPart,
      quantity: availableUnits,
    }));

    setQuantity(0);
    console.log(`Added ${quantity} of ${selectedPart.description} to the order.`);
  };

  // Handle Change in quantity
  const handleQuantityChange = (e) => {
    const newQuantity = parseInt(e.target.value, 10) || 0;

    // Assuming selectedPart is the part currently selected in your state
    if (selectedPart && newQuantity <= selectedPart.quantity) {
      setQuantity(newQuantity);
      setError('');
    } else {
      // If the entered quantity is invalid, set an error message
      setError(`Sorry! That's more than the available quantity of ${selectedPart.quantity}`);
    }
  };

  // Submit order to API
  const submitOrder = async () => {
    const orderResponse = await PlaceOrder(orderLines);

    setPlacedOrders((prevPlacedOrders) => [
      ...prevPlacedOrders,
      orderResponse,
    ]);

    fetchData(); // Get accurate data after submitting order
    clearOrderSummary();
  }

  const clearOrderSummary = () => {
    setOrderLines([]);
  }

  return (
    <div>
      <h1>Select Part</h1>
      <table>
        <thead>
          <tr>
            <th>Part</th>
            <th>Price per unit</th>
            <th>Quantity</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>
              <select
                value={selectedPart ? selectedPart.id : ''}
                onChange={(e) => {
                  const id = e.target.value;
                  const selectedPart = parts.find((part) => part.id === parseInt(id, 10));
                  setSelectedPart(selectedPart);
                }}
              >
                <option value="" disabled>Select a part</option>
                {parts
                  .filter((part) => part.quantity > 0)
                  .map((part) => (
                    <option key={part.id} value={part.id}>
                      {part.description}
                    </option>
                  ))}
              </select>
            </td>
            <td>{selectedPart && `$${selectedPart.price.toFixed(2)}`}</td>
            <td>
              <input
                type="number"
                value={quantity}
                onChange={handleQuantityChange}
              />
            </td>
            <td>
              <button onClick={addToOrder} disabled={!selectedPart || quantity <= 0}>
                Add to Order
              </button>
            </td>
          </tr>
        </tbody>
        {error && <p style={{ color: 'red' }}>{error}</p>}
      </table>

      {/* this should have an order ID, but as we're not databasing, I haven't handled this */}
      <h1>Order Summary</h1>
      <table>
        <thead>
          <tr>
            <th>Part ID</th>
            <th>Part Name</th>
            <th>Quantity</th>
            <th>Total</th>
          </tr>
        </thead>
        <tbody>
          {orderLines.map((item) => (
            <tr key={item.PartId}>
              <td>{item.PartId}</td>
              <td>{parts.find((part) => part.id === parseInt(item.PartId, 10)).description}</td>
              <td>{item.OrderQuantity}</td>
              <td>{(parts.find((part) => part.id === parseInt(item.PartId, 10)).price * item.OrderQuantity).toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
        <tfoot>
          <tr>
            <td colSpan="3">Total:</td>
            <td>{totalOrderCost.toFixed(2)}</td>
          </tr>
        </tfoot>
      </table>
      <button onClick={submitOrder}>Submit Order</button>

      {placedOrders[0] && Array.isArray(placedOrders[0].orderLines) && (
        <div>
          <h1>Order Summary</h1>
          <table>
            <thead>
              <tr>
                <th>Part ID</th>
                <th>Part Name</th>
                <th>Total</th>
              </tr>
            </thead>
            {placedOrders.map((order, index) => (
              <tbody key={order.id}>
                {order.orderLines.map((item) => (
                  <tr key={item.id} className={`orderline-${index % 2 === 0 ? 1 : 2}`}>
                    <td>{item.id}</td>
                    <td>{item.description}</td>
                    <td>{item.totalPrice.toFixed(2)}</td>
                  </tr>
                ))}
                <tr className={`total-${index % 2 === 0 ? 1 : 2}`}>
                  <td colSpan="2" >Total:</td>
                  <td>{order.total.toFixed(2)}</td>
                </tr>
              </tbody>
            ))}
          </table>
        </div>
      )}
    </div>
  );
};

export default OrderScreen;

