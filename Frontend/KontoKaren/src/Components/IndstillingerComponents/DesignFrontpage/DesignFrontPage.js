import { Box, Button, Typography, FormControlLabel, Checkbox } from '@mui/material';
import React, { useState, useEffect } from 'react';

const API_URL = 'http://localhost:5168/api/Indstillinger'; // Update with the actual endpoint

const getAuthToken = () => localStorage.getItem('authToken'); // Make sure you have an auth token saved in localStorage

function DesignFrontPage() {
    // State for each setting
    const [settings, setSettings] = useState({
        showPieChart: false,
        showBudget: false,
        showIndtægter: false,
        showUdgifter: true,
        showSøjlediagram: false,
    });

    // Fetch settings from the backend or localStorage
    useEffect(() => {
        const fetchSettings = async () => {
            const savedSettings = localStorage.getItem('designSettings');
            if (savedSettings) {
                setSettings(JSON.parse(savedSettings));
            } else {
                await fetchSettingsFromBackend();
            }
        };
        fetchSettings();
    }, []);

    const fetchSettingsFromBackend = async () => {
        try {
            const token = getAuthToken();
            if (token) {
                const response = await fetch(`${API_URL}/GetIndstillinger`, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                    },
                });

                if (response.ok) {
                    const data = await response.json();
                    const settingsFromBackend = {
                        showPieChart: data.showPieChart ?? false,
                        showBudget: data.showBudget ?? false,
                        showIndtægter: data.showIndtægter ?? false,
                        showUdgifter: data.showUdgifter ?? false,
                        showSøjlediagram: data.showSøjlediagram ?? false,
                    };
                    setSettings(settingsFromBackend);
                    console.log('settings fetched from backend: ', settingsFromBackend);
                    localStorage.setItem('designSettings', JSON.stringify(settingsFromBackend)); // Cache settings locally
                } else {
                    console.error('Failed to fetch settings');
                }
            }
        } catch (error) {
            console.error('Error fetching settings from backend:', error);
        }
    };

    // Save settings to the backend
    const handleSaveSettings = async () => {
        try {
            const token = getAuthToken();
            const id = 4; // **** Sat til 4, der er 4 indstillinger - lorteløsning:)
            if (token) {
                const response = await fetch(`${API_URL}/UpdateIndstillinger/${id}`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`,
                    },
                    body: JSON.stringify({
                        SetPieChart: settings.showPieChart,
                        SetBudget: settings.showBudget,
                        SetIndtægter: settings.showIndtægter,
                        SetUdgifter: settings.showUdgifter,
                        SetSøjlediagram: settings.showSøjlediagram,
                    }),
                    
                });
                console.log('gemt i backend', JSON.stringify({
                    SetPieChart: settings.showPieChart,
                    SetBudget: settings.showBudget,
                    SetIndtægter: settings.showIndtægter,
                    SetUdgifter: settings.showUdgifter,
                    SetSøjlediagram: settings.showSøjlediagram,
                }));


                if (response.ok) {
                    alert('Settings saved!');
                    localStorage.setItem('designSettings', JSON.stringify(settings)); // Update local cache
                } else {
                    console.error('Failed to save settings');
                }
            }
        } catch (error) {
            console.error('Error saving settings to backend:', error);
        }
    };
        const handleSettingChange = (name) => (event) => {
            setSettings((prevSettings) => ({
                ...prevSettings,
                [name]: event.target.checked,
            }));
        };
    
        return (
            <Box sx={{ padding: 4 }}>
                <Typography variant="h6" gutterBottom>
                    Vælg hvilke ting der skal vises på forsiden:
                </Typography>
    
                {/* Checkboxes for each setting */}
                <Box>
                    <FormControlLabel
                        control={<Checkbox checked={settings.showPieChart} onChange={handleSettingChange('showPieChart')} />}
                        label="Vis PieChart"
                    />
                </Box>
                <Box>
                    <FormControlLabel
                        control={<Checkbox checked={settings.showBudget} onChange={handleSettingChange('showBudget')} />}
                        label="Vis Budget"
                    />
                </Box>
                <Box>
                    <FormControlLabel
                        control={<Checkbox checked={settings.showIndtægter} onChange={handleSettingChange('showIndtægter')} />}
                        label="Vis Indtægter"
                    />
                </Box>
                <Box>
                    <FormControlLabel
                        control={<Checkbox checked={settings.showUdgifter} onChange={handleSettingChange('showUdgifter')} />}
                        label="Vis Udgifter"
                    />
                </Box>
                <Box>
                    <FormControlLabel
                        control={<Checkbox checked={settings.showSøjlediagram} onChange={handleSettingChange('showSøjlediagram')} />}
                        label="Vis Søjlediagram"
                    />
                </Box>
    
                {/* Save Button */}
                <Box>
                    <Button variant="contained" color="primary" onClick={handleSaveSettings}>
                        Gem indstillinger
                    </Button>
                </Box>
            </Box>
        );
}

export default DesignFrontPage;
