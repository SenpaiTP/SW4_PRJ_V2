const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5168/api/KategoriLimit';


//get funktion
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
        "Authorization": `Bearer ${token}/{id}`,
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      // Handle error response
      const errorText = await response.text();
      throw new Error(`Server error: ${errorText}`);
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error fetching AmountLimit:", error);
    return null;
  }
}

///post funktion
export async function createAmoutLimit(kategoryId, limit){
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
        body: JSON.stringify({kategoryId, limit}),
        });
        if (!response.ok) {
          // Handle error response
          const errorText = await response.text();
          throw new Error(`Server error: ${errorText}`);
        }

        const data = await response.json(); 
        return data;
    } catch (error) {
      console.error("Error creating AmountLimit:", error);
      return null;
    }  
}