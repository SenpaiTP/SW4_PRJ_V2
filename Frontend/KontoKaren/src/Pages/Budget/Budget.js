import { Container, Typography, TextField, Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, LinearProgress, Box, IconButton, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from "@mui/material";
import { Edit, Delete } from "@mui/icons-material";
import React, { useState, useEffect } from "react";
import dayjs from 'dayjs';

const API_URL = 'http://localhost:5168/api';

const getAuthToken = () => localStorage.getItem('authToken');

function Budget() {
  const [rows, setRows] = useState([]);
  const [newRow, setNewRow] = useState({ id: "", name: "", price: "", goalEndDate: "", saved: 0 });
  const [open, setOpen] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [selectedGoal, setSelectedGoal] = useState(null);
  const [savingsAmount, setSavingsAmount] = useState('');
  const [savingsDate, setSavingsDate] = useState(dayjs().format('YYYY-MM-DD'));
  const [savingsHistory, setSavingsHistory] = useState([]);

  useEffect(() => {
    fetchBudgets();
  }, []);

  const fetchBudgets = async () => {
    const response = await fetch(`${API_URL}/budget`, {
      headers: {
        'Authorization': `Bearer ${getAuthToken()}`
      }
    });
    const data = await response.json();
    setRows(data);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setNewRow((prevRow) => ({
      ...prevRow,
      [name]: value,
    }));
  };

  const handleAddRow = async () => {
    const today = dayjs();
    const endDate = dayjs(newRow.goalEndDate);
  
    if (!endDate.isValid() || endDate.isBefore(today, 'day')) {
      alert("Please choose a future date for the saving goal.");
      return;
    }
  
    if (editMode) {
      await fetch(`${API_URL}/budget/${newRow.id}/update`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${getAuthToken()}`
        },
        body: JSON.stringify(newRow)
      });
    } else {
      await fetch(`${API_URL}/budget`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${getAuthToken()}`
        },
        body: JSON.stringify(newRow)
      });
    }
    fetchBudgets();
    setNewRow({ id: "", name: "", price: "", goalEndDate: "", saved: 0 });
    setOpen(false);
    setEditMode(false);
  };

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setEditMode(false);
    setNewRow({ id: "", name: "", price: "", goalEndDate: "", saved: 0 });
  };

  const handleRowClick = async (row) => {
    const response = await fetch(`${API_URL}/budget/${row.id}`, {
      headers: {
        'Authorization': `Bearer ${getAuthToken()}`
      }
    });
    const data = await response.json();
    setSelectedGoal(data);
  };

  const handleEditGoal = (row) => {
    setNewRow(row);
    setEditMode(true);
    setOpen(true);
  };

  const handleDeleteGoal = async (id) => {
    await fetch(`${API_URL}/budget/${id}/delete`, {
      method: 'DELETE',
      headers: {
        'Authorization': `Bearer ${getAuthToken()}`
      }
    });
    fetchBudgets();
    if (selectedGoal && selectedGoal.id === id) {
      setSelectedGoal(null);
    }
  };

  const calculateProgress = (saved, price) => {
    return (saved / price) * 100;
  };

  const calculateSavings = (price, goalEndDate) => {
    const startDate = dayjs();
    const endDate = dayjs(goalEndDate);
    const months = endDate.diff(startDate, 'month') + 1; // Include the current month
    const weeks = endDate.diff(startDate, 'week') + 1; // Include the current week
    const days = endDate.diff(startDate, 'day') + 1; // Include the current day
    const monthlySavings = price / months;
    const weeklySavings = price / weeks;
    const dailySavings = price / days;
    return { monthlySavings, weeklySavings, dailySavings };
  };

  const handleAddSavings = async () => {
    if (selectedGoal && savingsAmount) {
      const newSavings = { amount: parseFloat(savingsAmount), date: savingsDate, type: 'add' };
      // Assuming you have an endpoint to add savings
      await fetch(`${API_URL}/budget/${selectedGoal.id}/add-savings`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${getAuthToken()}`
        },
        body: JSON.stringify(newSavings)
      });
      fetchBudgets();
      setSavingsHistory((prevHistory) => [...prevHistory, { ...newSavings, goalId: selectedGoal.id }]);
      setSavingsAmount('');
      setSavingsDate(dayjs().format('YYYY-MM-DD'));
    }
  };

  const handleRemoveSavings = async () => {
    if (selectedGoal && savingsAmount) {
      const newSavings = { amount: parseFloat(savingsAmount), date: savingsDate, type: 'remove' };
      // Assuming you have an endpoint to remove savings
      await fetch(`${API_URL}/budget/${selectedGoal.id}/remove-savings`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${getAuthToken()}`
        },
        body: JSON.stringify(newSavings)
      });
      fetchBudgets();
      setSavingsHistory((prevHistory) => [...prevHistory, { ...newSavings, goalId: selectedGoal.id }]);
      setSavingsAmount('');
      setSavingsDate(dayjs().format('YYYY-MM-DD'));
    }
  };

  return (
    <Container>
      <Typography variant="h1" component="h2" gutterBottom color="text.primary">
        Welcome to Budget
      </Typography>

      <Box display="flex" flexDirection="row" marginTop="20px">
        <Box flex={1} display="flex" flexDirection="column" alignItems="flex-start">
          <Typography variant="h3" gutterBottom color="text.primary">Liste over opsparingsmål</Typography>
          <TableContainer component={Paper} style={{ marginBottom: '20px' }}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>ID</TableCell>
                  <TableCell>Navn</TableCell>
                  <TableCell>Pris (DKK)</TableCell>
                  <TableCell>Mål Slutdato</TableCell>
                  <TableCell>Handlinger</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {rows.map((row) => (
                  <TableRow key={row.id} onClick={() => handleRowClick(row)} style={{ cursor: 'pointer' }}>
                    <TableCell>{row.id}</TableCell>
                    <TableCell>{row.name}</TableCell>
                    <TableCell>{row.price}</TableCell>
                    <TableCell>{dayjs(row.goalEndDate).format('DD/MM/YYYY')}</TableCell>
                    <TableCell>
                      <IconButton onClick={() => handleEditGoal(row)}><Edit /></IconButton>
                      <IconButton onClick={() => handleDeleteGoal(row.id)}><Delete /></IconButton>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
          <Button variant="contained" color="primary" onClick={handleClickOpen}>
            Add New Saving Goal
          </Button>
        </Box>

        {selectedGoal && (
          <Box flex={1} marginLeft="20px">
            <Typography variant="h4" gutterBottom color="text.primary">Progress for {selectedGoal.name}</Typography>
            <Box height="20px" marginTop="10px">
              <LinearProgress variant="determinate" value={calculateProgress(selectedGoal.saved, selectedGoal.price)} style={{ height: '20px' }} />
            </Box>
            <Typography>{selectedGoal.saved} / {selectedGoal.price} DKK saved</Typography>
            <Box marginTop="20px" padding="10px" border="1px solid #ccc" borderRadius="5px">
              <Typography variant="h5">Savings Plan</Typography>
              <Typography>
                To save up for {selectedGoal.name}, you need to save approximately {calculateSavings(selectedGoal.price, selectedGoal.goalEndDate).monthlySavings.toFixed(2)} DKK per month.
              </Typography>
              <Typography>
                This is equivalent to approximately {calculateSavings(selectedGoal.price, selectedGoal.goalEndDate).weeklySavings.toFixed(2)} DKK per week.
              </Typography>
              <Typography>
                This is equivalent to approximately {calculateSavings(selectedGoal.price, selectedGoal.goalEndDate).dailySavings.toFixed(2)} DKK per day.
              </Typography>
              <Typography>
                You have {dayjs(selectedGoal.goalEndDate).diff(dayjs(), 'day')} days left to reach your goal.
              </Typography>
              <Typography>
                You have saved {calculateProgress(selectedGoal.saved, selectedGoal.price).toFixed(2)}% of your goal.
              </Typography>
            </Box>

            <Box marginTop="20px" padding="10px" border="1px solid #ccc" borderRadius="5px">
              <Typography variant="h5">Add to Savings</Typography>
              <TextField
                label="Amount Saved"
                type="number"
                value={savingsAmount}
                onChange={(e) => setSavingsAmount(e.target.value)}
                fullWidth
                margin="dense"
              />
              <TextField
                label="Date"
                type="date"
                value={savingsDate}
                onChange={(e) => setSavingsDate(e.target.value)}
                fullWidth
                margin="dense"
                InputLabelProps={{
                  shrink: true,
                }}
              />
              <Box display="flex" justifyContent="space-between" mt={2}>
                <Button variant="contained" color="primary" onClick={handleAddSavings} sx={{ mr: 1 }}>
                  Add Savings
                </Button>
                <Button variant="contained" color="secondary" onClick={handleRemoveSavings}>
                  Remove Savings
                </Button>
              </Box>
            </Box>

            <Box marginTop="20px" padding="10px" border="1px solid #ccc" borderRadius="5px">
              <Typography variant="h5">Savings History</Typography>
              <TableContainer component={Paper}>
                <Table>
                  <TableHead>
                    <TableRow>
                      <TableCell>Date</TableCell>
                      <TableCell>Amount</TableCell>
                      <TableCell>Type</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {savingsHistory.filter(entry => entry.goalId === selectedGoal.id).map((entry, index) => (
                      <TableRow key={index}>
                        <TableCell>{entry.date}</TableCell>
                        <TableCell>{entry.amount} DKK</TableCell>
                        <TableCell>{entry.type}</TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </TableContainer>
            </Box>
          </Box>
        )}
      </Box>

      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>{editMode ? "Edit Saving Goal" : "Add New Saving Goal"}</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Please fill out the form below to {editMode ? "edit" : "add"} a saving goal.
          </DialogContentText>
          <TextField
            autoFocus
            margin="dense"
            label="Name of the Thing"
            name="name"
            value={newRow.name}
            onChange={handleChange}
            fullWidth
          />
          <TextField
            margin="dense"
            label="Price"
            name="price"
            value={newRow.price}
            onChange={handleChange}
            fullWidth
          />
          <TextField
            margin="dense"
            label="Goal End Date"
            name="goalEndDate"
            type="date"
            value={newRow.goalEndDate}
            onChange={handleChange}
            fullWidth
            InputLabelProps={{
              shrink: true,
            }}
            inputProps={{
              min: dayjs().format('YYYY-MM-DD'), // Restrict to today's date and future dates
            }}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} color="primary">
            Cancel
          </Button>
          <Button onClick={handleAddRow} color="primary">
            {editMode ? "Save Changes" : "Add"}
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
}

export default Budget;