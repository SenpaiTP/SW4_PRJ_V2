// // Dynamisk API URL
// const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5168/api/KategoriLimit';

// /**
//  * Funktion til at hente data med GET
//  * @param {number} kategoryId - Kategoriens ID
//  * @param {string} kategoryName - Kategoriens navn
//  * @param {number} limit - Begrænsning (hvis påkrævet i query)
//  * @returns {Object|null} - Returnerer data fra serveren eller null ved fejl
//  */
// export async function fetchAmountLimit(kategoryId, kategoryName, limit) {
//   const token = localStorage.getItem("authToken");

//   if (!token) {
//     console.error("No auth token found.");
//     return null;
//   }

//   try {
//     // Konstruer query parameters
//     const queryParams = new URLSearchParams({
//       kategoryId: kategoryId.toString(),
//       kategoryName: kategoryName,
//       limit: limit.toString(),
//     }).toString();

//     // Foretag GET-anmodning
//     const response = await fetch(`${API_URL}?${queryParams}`, {
//       method: "GET",
//       headers: {
//         "Authorization": `Bearer ${token}`,
//         "Content-Type": "application/json",
//       },
//     });

//     if (!response.ok) {
//       const errorText = await response.text();
//       throw new Error(`Server error: ${errorText}`);
//     }

//     const data = await response.json();
//     console.log('GET response data:', data);

//     return data;
//   } catch (error) {
//     console.error('Error fetching amount limit:', error.message);
//     return null;
//   }
// }

// /**
//  * Funktion til at sende data med POST
//  * @param {number} kategoryId - Kategoriens ID
//  * @param {number} limit - Beløbsgrænsen
//  * @returns {Object|null} - Returnerer data fra serveren eller null ved fejl
//  */
// export async function setAmountLimit(kategoryId, limit) {
//   const token = localStorage.getItem("authToken");

//   if (!token) {
//     console.error("No auth token found.");
//     return null;
//   }

//   try {
//     // Foretag POST-anmodning
//     const response = await fetch(API_URL, {
//       method: "POST",
//       headers: {
//         "Authorization": `Bearer ${token}`,
//         "Content-Type": "application/json",
//       },
//       body: JSON.stringify({
//         kategoryId: kategoryId,
//         limit: limit,
//       }),
//     });

//     if (!response.ok) {
//       const errorText = await response.text();
//       throw new Error(`Server error: ${errorText}`);
//     }

//     const data = await response.json();
//     console.log('POST response data:', data);

//     return data;
//   } catch (error) {
//     console.error('Error setting amount limit:', error.message);
//     return null;
//   }
// }
