const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5168/api/Kategori';

export async function suggestCategory(description) {
  const token = localStorage.getItem("authToken");

  if (!token) {
    console.error("No auth token found.");
    return null;
  }

  try {
    const response = await fetch(`${API_URL}/suggest`, {
      method: "POST",
      headers: {
        "Authorization": `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ description }), // Send only description
    });

    if (!response.ok) {
      // Handle error response
      const errorText = await response.text();
      throw new Error(`Server error: ${errorText}`);
    }

    // Check the content type of the response
    const contentType = response.headers.get("Content-Type");

    let suggestedCategory;

    if (contentType && contentType.includes("application/json")) {
      // If the response is JSON, try to parse it
      suggestedCategory = await response.json();
    } else if (contentType && contentType.includes("text/plain")) {
      // If the response is plain text (like "Miscellaneous"), return it as a string
      suggestedCategory = await response.text();
    } else {
      // Handle unexpected content types
      throw new Error("Unexpected response format: Neither JSON nor plain text.");
    }

    console.log('Suggested category:', description, ":", suggestedCategory);

    return suggestedCategory;
  } catch (error) {
    console.error('Error suggesting category:', error.message);
    return null;
  }
}

export const handleSuggestCategoriesService = (rows, categoryOptions, setCategoryOptions, currentCategories) => {
  if (!rows || rows.length === 0) {
    console.error('Ingen rækker til at foreslå kategorier');
    return;
  }

  // Ensure categoryOptions is an array
  if (!Array.isArray(categoryOptions)) {
    console.error('categoryOptions er ikke et array!');
    setCategoryOptions([]); // Reset categoryOptions to an empty array if it's not
    return;
  }

  rows.forEach((row) => {
    // Use row.name as description, as they are the same
    const descriptionName = { description: row.name };

    suggestCategory(descriptionName.description).then((category) => {
      if (category) {
        console.log(`Navn: ${row.name}, Foreslået kategori: ${category}`);

        // Check if the category already exists
        const categoryExists = categoryOptions.some(
          (existingCategory) => existingCategory.categoryName.toLowerCase() === category.toLowerCase()
        );

        if (!categoryExists) {
          const newCategory = { categoryName: category };
          setCategoryOptions((prevOptions) => [...prevOptions, newCategory]);
          console.log(`Ny kategori tilføjet: ${category}`);
        }
      }
    }).catch(error => {
      console.error('Fejl ved foreslået kategori:', error.message);
    });
  });
};