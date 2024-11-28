import React from "react";
import { Menu, MenuItem } from "@mui/material";

export default function TableActionMenu({
  anchorEl,
  onCloseMenu,
  onAddRow,
  onEditRow,
  onDeleteRow,
}) {
  return (
    <Menu
      anchorEl={anchorEl}
      open={Boolean(anchorEl)}
      onClose={onCloseMenu}
    >
      <MenuItem onClick={onAddRow}>Tilføj indtægt</MenuItem>
      <MenuItem onClick={onEditRow}>Rediger indtægt</MenuItem>
      <MenuItem onClick={onDeleteRow}>Slet indtægt</MenuItem>
    </Menu>
  );
}
