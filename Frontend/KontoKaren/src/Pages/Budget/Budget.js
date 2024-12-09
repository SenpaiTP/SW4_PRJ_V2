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
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    fetchBudgets();
  }, []);

  const fetchBudgets = async () => {
    setLoading(true);
    try {
      const response = await fetch(`${API_URL}/Budget/AllBudgets`, {
        headers: {
          'Authorization': `Bearer ${getAuthToken()}`
        }
      });
      if (!response.ok) throw new Error("Failed to fetch budgets");
      const rawData = await response.json();
      const formattedData = rawData.map(item => ({
        id: item.id || item.budgetId,
        name: item.budgetName,
        price: item.savingsGoal,
        goalEndDate: item.budgetSlut,
        saved: item.saved || 0,
      }));
      setRows(formattedData);
    } catch (error) {
      console.error("Error fetching budgets:", error);
      alert("Failed to load budgets.");
    } finally {
      setLoading(false);
    }
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
      alert("Vælg venligst en fremtidig dato for opsparingsmålet.");
      return;
    }
  
    const savingGoal = {
      budgetName: newRow.name,
      savingsGoal: parseFloat(newRow.price),
      budgetSlut: newRow.goalEndDate,
    };
  
    try {
      // Determine if we're in edit mode and set the URL accordingly
      const url = editMode ? `${API_URL}/Budget/${newRow.id}/UpdateBudget` : `${API_URL}/Budget/NewBudget`;
      
      const response = await fetch(url, {
        method: editMode ? 'PUT' : 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${getAuthToken()}`,
        },
        body: JSON.stringify(editMode ? { ...savingGoal, id: newRow.id } : savingGoal),
      });
  
      if (!response.ok) throw new Error("Failed to save the saving goal");
  
      fetchBudgets();  // Refresh the list of goals
      setNewRow({ id: "", name: "", price: "", goalEndDate: "", saved: 0 });
      setOpen(false);
      setEditMode(false);
    } catch (error) {
      console.error("Error saving goal:", error);
      alert("Failed to save the goal.");
    }
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
    try {
      const response = await fetch(`${API_URL}/Budget/${row.id}`, {
        headers: {
          'Authorization': `Bearer ${getAuthToken()}`,
        },
      });
      if (!response.ok) throw new Error("Failed to fetch goal details");
      const data = await response.json();
  
      setSelectedGoal({
        id: data.id,
        name: data.budgetName,
        price: data.savingsGoal,
        goalEndDate: data.budgetSlut,
        saved: data.saved || 0,
      });
  
      // Fetch savings history for the goal
      const historyResponse = await fetch(`${API_URL}/Saving/${row.id}`, {
        headers: {
          'Authorization': `Bearer ${getAuthToken()}`,
        },
      });
      if (!historyResponse.ok) throw new Error("Failed to fetch savings history");
      const historyData = await historyResponse.json();
      setSavingsHistory(historyData);
    } catch (error) {
      console.error("Error fetching goal details or history:", error);
      alert("Failed to load goal details.");
    }
  };
  
  

  const handleEditGoal = (row) => {
    setNewRow({
      id: row.id,
      name: row.name,
      price: row.price,
      goalEndDate: row.goalEndDate,
      saved: row.saved,
    });
    setEditMode(true);
    setOpen(true);
  };

  const handleDeleteGoal = async (id) => {
    try {
        const response = await fetch(`${API_URL}/Budget/${id}/DeleteBudget`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${getAuthToken()}`,
            },
        });

        if (!response.ok) {
            throw new Error("Failed to delete goal");
        }

        fetchBudgets();
        alert("Goal deleted successfully");
    } catch (error) {
        console.error("Error deleting goal:", error);
        alert("Failed to delete goal.");
    }
};


  const calculateProgress = (saved, price) => {
    return (saved / price) * 100;
  };

  const calculateSavings = (price, goalEndDate) => {
    const startDate = dayjs();
    const endDate = dayjs(goalEndDate);
    const months = endDate.diff(startDate, 'month') + 1;
    const weeks = endDate.diff(startDate, 'week') + 1;
    const days = endDate.diff(startDate, 'day') + 1;
    return {
      monthlySavings: price / months,
      weeklySavings: price / weeks,
      dailySavings: price / days,
    };
  };

  const handleAddSavings = async () => {
    if (!selectedGoal || !selectedGoal.id) {
      alert("Please select a goal before adding savings.");
      return;
    }
  
    if (!savingsAmount || parseFloat(savingsAmount) <= 0) {
      alert("Please enter a valid savings amount.");
      return;
    }
  
    try {
      const newSavings = {
        amount: parseFloat(savingsAmount),
        date: savingsDate,
      };
  
      const response = await fetch(`${API_URL}/Saving/${selectedGoal.id}`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${getAuthToken()}`,
        },
        body: JSON.stringify(newSavings),
      });
  
      if (!response.ok) throw new Error("Failed to add savings.");
  
      fetchBudgets(); // Refresh the list of goals
      alert("Savings added successfully.");
      setSavingsAmount('');
      setSavingsDate(dayjs().format('YYYY-MM-DD')); // Reset input fields
    } catch (error) {
      console.error("Error adding savings:", error);
      alert("Failed to add savings.");
    }
  };
  
  
  const handleRemoveSavings = async (savingId) => {
    if (!selectedGoal || !selectedGoal.id) {
      alert("Please select a goal before removing savings.");
      return;
    }
  
    if (!savingId) {
      alert("Please enter a valid savings ID.");
      return;
    }
  
    try {
      const response = await fetch(`${API_URL}/Saving/${selectedGoal.id}/${savingId}`, {
        method: "DELETE",
        headers: {
          "Authorization": `Bearer ${getAuthToken()}`,
        },
      });
  
      if (!response.ok) throw new Error("Failed to remove savings.");
  
      fetchBudgets(); // Refresh the list of goals
      alert("Savings removed successfully.");
    } catch (error) {
      console.error("Error removing savings:", error);
      alert("Failed to remove savings.");
    }
  };
  

  return (
    <Container>
      <Typography variant="h1" component="h2" gutterBottom>
        Welcome to Budget
      </Typography>

      <Box display="flex" flexDirection="row" marginTop="20px">
        <Box flex={1} display="flex" flexDirection="column" alignItems="flex-start">
          <Typography variant="h3" gutterBottom>Liste over opsparingsmål</Typography>
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
                    <IconButton onClick={() => handleEditGoal(row, row.id)}><Edit /></IconButton>
                    <IconButton onClick={() => handleDeleteGoal(row.id)}><Delete /></IconButton>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
          <Button variant="contained" color="primary" onClick={handleClickOpen}>
            Opret nyt opsparingsmål
          </Button>
        </Box>

        {selectedGoal && (
          <Box flex={1} marginLeft="20px">
            <Typography variant="h4" gutterBottom>Fremskridt for {selectedGoal.name}</Typography>
            <Box height="20px" marginTop="10px">
              <LinearProgress variant="determinate" value={calculateProgress(selectedGoal.saved, selectedGoal.price)} style={{ height: '20px' }} />
            </Box>
            <Typography>{selectedGoal.saved} / {selectedGoal.price} DKK saved</Typography>
            <Box marginTop="20px" padding="10px" border="1px solid #ccc" borderRadius="5px">
              <Typography variant="h5">Savings Plan</Typography>
              <Typography>
              For at spare op til {selectedGoal.name}, skal du spare cirka {calculateSavings(selectedGoal.price, selectedGoal.goalEndDate).monthlySavings.toFixed(2)} DKK per måned.
              </Typography>
              <Typography>
                Det er tilsvarende til {calculateSavings(selectedGoal.price, selectedGoal.goalEndDate).weeklySavings.toFixed(2)} DKK per uge.
              </Typography>
              <Typography>
                Det er tilsvarende til {calculateSavings(selectedGoal.price, selectedGoal.goalEndDate).dailySavings.toFixed(2)} DKK per dag.
              </Typography>
              <Typography>
                Du har {dayjs(selectedGoal.goalEndDate).diff(dayjs(), 'day')} dage tilbage til at nå dit mål.
              </Typography>
              <Typography>
                Du har opsaret {calculateProgress(selectedGoal.saved, selectedGoal.price).toFixed(2)}% af dit mål.
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