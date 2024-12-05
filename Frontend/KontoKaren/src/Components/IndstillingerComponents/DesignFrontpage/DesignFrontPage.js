import { Box, Button, Typography, FormControlLabel, Checkbox } from '@mui/material';
import React, { useState, useEffect } from 'react';

const API_URL = 'http://localhost:5168/api/Indstillinger'; // Update with the actual endpoint

const getAuthToken = () => localStorage.getItem('authToken'); // Make sure you have an auth token saved in localStorage

function DesignFrontPage() {
    // State for each setting
    const [showPieChart, setShowPieChart] = useState(true);
    const [showBudget, setShowBudget] = useState(true);
    const [showIndtægter, setShowIndtægter] = useState(true);
    const [showUdgifter, setShowUdgifter] = useState(true);
    const [showSøjlediagram, setShowSøjlediagram] = useState(true);


    // Fetch settings from the backend
    useEffect(() => {
        const fetchSettings = async () => {
            const token = getAuthToken();
            if (token) {
                const response = await fetch(`${API_URL}`, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                });

                if (response.ok) {
                    const data = await response.json();
                    // Assuming the response has a structure similar to { showPieChart, showBudget, showIndtægter, ... }
                    setShowPieChart(data.showPieChart ?? false);
                    setShowBudget(data.showBudget ?? true);
                    setShowIndtægter(data.showIndtægter ?? false);
                    setShowUdgifter(data.showUdgifter ?? true);
                    setShowSøjlediagram(data.showSøjlediagram ?? true);
                } else {
                    console.error('Failed to fetch settings');
                }
            }
        };
        fetchSettings();
    }, []);

    // Save settings to the backend
    const handleSaveSettings = async () => {
        const token = getAuthToken();
        if (token) {
            const response = await fetch(`${API_URL}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({
                    showPieChart,
                    showBudget,
                    showIndtægter,
                    showUdgifter,
                    showSøjlediagram
                })
            });

            if (response.ok) {
                alert('Settings saved!');
            } else {
                console.error('Failed to save settings');
            }
        }
    };

    return (
        <Box sx={{ padding: 4 }}>
            <Typography variant="h7" gutterBottom>
                Vælg hvilke ting der skal vises på forsiden:
            </Typography>

            {/* Form control checkboxes for each setting */}
            <Box>
                <FormControlLabel
                    control={<Checkbox checked={showPieChart} onChange={(e) => setShowPieChart(e.target.checked)} />}
                    label="Vis PieChart" />
            </Box>

            <Box>
                <FormControlLabel
                    control={<Checkbox checked={showBudget} onChange={(e) => setShowBudget(e.target.checked)} />}
                    label="Vis Budget" />
            </Box>

            <Box>
                <FormControlLabel
                    control={<Checkbox checked={showIndtægter} onChange={(e) => setShowIndtægter(e.target.checked)} />}
                    label="Vis Indtægter" />
            </Box>

            <Box>
                <FormControlLabel
                    control={<Checkbox checked={showUdgifter} onChange={(e) => setShowUdgifter(e.target.checked)} />}
                    label="Vis Udgifter" />
            </Box>

            <Box>
                <FormControlLabel
                    control={<Checkbox checked={showSøjlediagram} onChange={(e) => setShowSøjlediagram(e.target.checked)} />}
                    label="Vis Søjlediagram" />
            </Box>

            <Box>
                <Button variant="contained" color="primary" onClick={handleSaveSettings}>
                    Gem indstillinger
                </Button>
            </Box>
            <Typography variant="h8" gutterBottom>
                Til senere skal ses om der kan gemmes uden en pop up der siger det er gemt
            </Typography>
        </Box>
    );
}

export default DesignFrontPage;
