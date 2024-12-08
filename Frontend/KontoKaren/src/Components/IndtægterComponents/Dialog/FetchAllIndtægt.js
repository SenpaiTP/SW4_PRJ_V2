import { useState, useEffect } from 'react';

export default function useFetchIncomes() {
    const [jobs, setJobs] = useState([]);
    const [errorMessage, setErrorMessage] = useState(null);

    useEffect(() => {
        const token = localStorage.getItem("authToken");

        const fetchAllIncome = async () => {
            try {
                const response = await fetch(`http://localhost:5168/api/Findtægt`, {
                    method: 'GET',
                    credentials: 'include',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                });

                if (!response.ok) {
                    throw new Error("Noget gik galt: " + response.status);
                }

                const data = await response.json();
                setJobs(data);
            } catch (error) {
                setErrorMessage("En fejl opstod: " + error.message);
            }
        };

        fetchAllIncome();
    }, []); // Runs once on mount

    return { jobs, errorMessage };
}