import React, { useState } from 'react';

const fetchIncomeById = async (id) => {
  const token = localStorage.getItem("authToken");
  try {
    const response = await fetch(`http://localhost:5168/api/Findtægt/id?id=${id}`, {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${token}`,
      },
    });
    if (!response.ok) {
      throw new Error('Kunne ikke hente indkomstdata fra serveren.');
    }
    const incomeData = await response.json();
    console.log("IncomeData", incomeData);
    return incomeData;
  } catch (error) {
    console.error('Fejl ved hentning af indkomst:', error);
    return null;
  }
};

export default fetchIncomeById;

