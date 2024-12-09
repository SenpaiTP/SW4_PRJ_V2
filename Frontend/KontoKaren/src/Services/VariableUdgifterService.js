const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5168/api/Vudgifter';


//get funktion
export async function getVudgifter() {
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

    const data = await response.json(); // Parse the response data
    return data;
  } catch (error) {
    console.error("Error fetching Vudgifter:", error);
    return null;
  }
}

///post funktion
export async function createVudgifter(vudgiftId, pris, tekst, kategoriNavn, dato){
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
        body: JSON.stringify({vudgiftId, pris, tekst, kategoriNavn, dato}),
        });
        if (!response.ok) {
          const errorText = await response.text();
          throw new Error(`Server error: ${errorText}`);
        }

        const data = await response.json(); 
        return data;
    } catch (error) {
      console.error("Error creating Vudgifter:", error);
      return null;
    }  
}

  //put funktion
  export async function updateVudgifter(id, pris, tekst, dato, kategoriId, kategoriNavn){
    const token = localStorage.getItem("authToken");

    if (!token) {
      console.error("No auth token found.");
      return null;
    }

    try {
      const response = await fetch(`${API_URL}/${id}`, {
        method: "PUT",
        headers: {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json",
        },
        body: JSON.stringify({pris, tekst, dato, kategoriId, kategoriNavn}),
        });
        if (!response.ok) {
          const errorText = await response.text();
          throw new Error(`Server error: ${errorText}`);
        }

        const data = await response.json(); 
        return data;
    } catch (error) {
      console.error("Error uptading Vudgifter:", error);
      return null;
    }
}


//delete funktion
export async function deleteVudgifter(id) {
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
  
      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Server error: ${errorText}`);
      }
  
      const data = await response.json(); 
      return data;
    } catch (error) {
      console.error("Error deleting Vudgift:", error);
      return null;
    }
  }