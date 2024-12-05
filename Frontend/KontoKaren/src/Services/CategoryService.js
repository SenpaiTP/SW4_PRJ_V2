
// Dynamisk API URL
const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5168/api/Kategori';

export default async function SuggestCategory(description) {
  const token = localStorage.getItem("authToken");

  
  try {
        const response = await fetch(`${API_URL}/suggest`, {
      method: "POST",
      headers: {
        "Authorization": `Bearer ${token}`, 
        "Content-Type": "application/json",
      },
          body: JSON.stringify({ description }), // Sender kun description
      });

      if (!response.ok) {
          // Håndter fejl her
          const errorText = await response.text();
          throw new Error(`Server error: ${errorText}`);
      }

      const suggestedCategory = await response.json(); // Håndter det returnerede resultat
      console.log('Suggested category:', suggestedCategory);

      return suggestedCategory;
  } catch (error) {
      console.error('Error suggesting category:', error.message);
      return null;
  }
}


