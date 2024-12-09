const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5168/api/KategoriLimit';

export async function getAmountLimit() {
  const token = localStorage.getItem("authToken");

  if (!token) {
    console.error("No auth token found.");
    return null;
  }

  try {
    const response = await fetch(`${API_URL}`, {
      method: "GET",
      headers: {
        "Authorization": `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`Server error: ${errorText}`);
    }

    const data = await response.json();
    return data.map(limit => ({
      kategoryId: limit.kategoryId,
      kategoryName: limit.kategoryName,
      limit: limit.limit
    }));
  } catch (error) {
    console.error("Error fetching AmountLimit:", error);
    return null;
  }
}

// get funktion med id
export async function getIDAmountLimit(id) {
  const token = localStorage.getItem("authToken");

  if (!token) {
    console.error("No auth token found.");
    return null;
  }

  try {
    const response = await fetch(`${API_URL}/${id}`, {
      method: "GET",
      headers: {
        "Authorization": `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`Server error: ${errorText}`);
    }

    const data = await response.json();
    return {
      kategoryId: data.kategoryId,
      kategoryName: data.kategoryName,
      limit: data.limit
    };
  } catch (error) {
    console.error("Error fetching ID AmountLimit:", error);
    return null;
  }
}

// post funktion
export async function setAmountLimit(kategoryId, limit) {
  const token = localStorage.getItem("authToken");

  if (!token) {
    console.error("No auth token found.");
    return null;
  }

  try {
    const response = await fetch(`${API_URL}`, {
      method: "POST",
      headers: {
        "Authorization": `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ kategoryId, limit }),
    });

    console.log('Response:', response);

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`Server error: ${errorText}`);
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error setting AmountLimit:", error);
    return null;
  }
}

// put funktion
export async function updateAmountLimit(kategoryId, limit) {
  const token = localStorage.getItem("authToken");

  if (!token) {
    console.error("No auth token found.");
    return null;
  }

  try {
    const response = await fetch(`${API_URL}/${kategoryId}`, {
      method: "PUT",
      headers: {
        "Authorization": `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ limit }),
    });

    console.log('Response:', response);

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`Server error: ${errorText}`);
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error updating AmountLimit:", error);
    return null;
  }
}

// delete funktion
export async function deleteAmountLimit(id) {
  const token = localStorage.getItem("authToken");

  if (!token) {
    console.error("No auth token found.");
    return null;
  }

  try {
    const response = await fetch(`${API_URL}/${id}`, {
      method: "DELETE",
      headers: {
        "Authorization": `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    });

    console.log('Response:', response);

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`Server error: ${errorText}`);
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error deleting AmountLimit:", error);
    return null;
  }
}