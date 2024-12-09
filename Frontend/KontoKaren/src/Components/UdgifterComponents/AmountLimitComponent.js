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
      // Refresh the limits after setting or updating a limit
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
    setCategoryId(limit.kategoryId);
    setLimit(limit.limit);
    setIsUpdating(true);
  };

  const handleDelete = async (kategoryId) => {
    const result = await deleteAmountLimit(kategoryId);
    console.log('Delete Result:', result);
    if (result) {
      setMessage('Limit deleted successfully!');
      // Refresh the limits after deleting a limit
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
      <h2>{isUpdating ? 'Update Amount Limit' : 'Set Amount Limit'}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>
            Kategori ID:
            <input
              type="text"
              value={categoryId}
              onChange={(e) => setCategoryId(e.target.value)}
              required
              readOnly={isUpdating} // Make the category ID read-only when updating
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
      <h2>Eksisterende Beløbsgrænser</h2>
      <ul>
        {limits.map((limit) => (
          <li key={limit.kategoryId}>
            {limit.kategoryName}: {limit.limit}
            <button onClick={() => handleEdit(limit)}>Edit</button>
            <button onClick={() => handleDelete(limit.kategoryId)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default AmountLimitComponent;