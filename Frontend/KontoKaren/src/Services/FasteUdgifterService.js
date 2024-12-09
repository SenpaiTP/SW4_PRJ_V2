const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5168/api/Fudgifter';


//get funktion
export async function getFudgifter() {
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
    return data;
  } catch (error) {
    console.error("Error fetching Fudgifter:", error);
    return null;
  }
}

///post funktion
export async function createFudgifter(fudgiftId, pris, tekst, kategoriNavn, dato){
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
        body: JSON.stringify({fudgiftId, pris, tekst, kategoriNavn, dato}),
        });
        if (!response.ok) {
          const errorText = await response.text();
          throw new Error(`Server error: ${errorText}`);
        }

        const data = await response.json(); 
    } catch (error) {
      console.error("Error creating Fudgifter:", error);
      return null;
    }  
}

  //put funktion
  export async function updateFudgifter(id, pris, tekst, dato, kategoriId, kategoriNavn){
    const token = localStorage.getItem("authToken");

    if (!token) {
      console.error("No auth token found.");
      return null;
    }

    try {
      const response = await fetch(`${API_URL}/opdater/${id}`, {
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
      console.error("Error uptading Fudgifter:", error);
      return null;
    }
}


//delete funktion
export async function deleteFudgifter(id) {
    const token = localStorage.getItem("authToken");
  
    if (!token) {
      console.error("No auth token found.");
      return null;
    }
  
    try {
      const response = await fetch(`${API_URL}/${id}/slet`, {
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
      console.error("Error deleting Fudgift:", error);
      return null;
    }
  }