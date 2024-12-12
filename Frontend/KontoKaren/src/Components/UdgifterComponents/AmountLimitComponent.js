import React, { useState, useEffect } from 'react';
import { getAmountLimit, setAmountLimit, updateAmountLimit, deleteAmountLimit } from '../../Services/AmountLimitService';

const AmountLimitComponent = () => {
  const [categoryId, setCategoryId] = useState('');
  const [limit, setLimit] = useState('');
  const [message, setMessage] = useState('');
  const [limits, setLimits] = useState([]);
  const [isUpdating, setIsUpdating] = useState(false);

  useEffect(() => {
    const fetchLimits = async () => {
      const data = await getAmountLimit();
      if (data) {
        setLimits(data);
      }
    };

    fetchLimits();
  }, []);

  const handleSubmit = async (event) => {
    event.preventDefault();
    let result;
    if (isUpdating) {
      result = await updateAmountLimit(categoryId, limit);
    } else {
      result = await setAmountLimit(categoryId, limit);
    }
    console.log('Result:', result);
    if (result) {
      setMessage(isUpdating ? 'Limit updated successfully!' : 'Limit set successfully!');
      const data = await getAmountLimit();
      if (data) {
        setLimits(data);
      }
    } else {
      setMessage(isUpdating ? 'Failed to update limit.' : 'Failed to set limit.');
    }
    setIsUpdating(false);
    setCategoryId('');
    setLimit('');
  };

  const handleEdit = (limit) => {
    setCategoryId(limit.CategoryId);
    setLimit(limit.limit);
    setIsUpdating(true);
  };

  const handleDelete = async (CategoryId) => {
    const result = await deleteAmountLimit(CategoryId);
    console.log('Delete Result:', result);
    if (result) {
      setMessage('Limit deleted successfully!');
      const data = await getAmountLimit();
      if (data) {
        setLimits(data);
      }
    } else {
      setMessage('Failed to delete limit.');
    }
  };

  return (
    <div>
      <h2>{isUpdating ? 'Update Amount Limit' : 'Sæt beløbsgrænse for kategori'}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>
            Kategori ID:
            <input
              type="text"
              value={categoryId}
              onChange={(e) => setCategoryId(e.target.value)}
              required
              readOnly={isUpdating} 
            />
          </label>
        </div>
        <div>
          <label>
            Beløbsgrænse:
            <input
              type="number"
              value={limit}
              onChange={(e) => setLimit(e.target.value)}
              required
            />
          </label>
        </div>
        <button type="submit">{isUpdating ? 'Update Limit' : 'Sæt Beløbsgrænse'}</button>
      </form>
      {message && <p>{message}</p>}
      <ul>
        {limits.map((limit) => (
          <li key={limit.CategoryId}>
            {limit.CategoryName}: {limit.limit}
            <button onClick={() => handleEdit(limit)}>Edit</button>
            <button onClick={() => handleDelete(limit.CategoryId)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default AmountLimitComponent;