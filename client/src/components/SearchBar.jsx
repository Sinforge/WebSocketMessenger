import { useState } from "react";
import IconButton from "@mui/material/IconButton";
import Icon from '@mui/material/Icon';
import TextField from "@mui/material/TextField";

const SearchBar = ({setSearchQuery}) => (
    <form>
      <TextField
        id="search-bar"
        className="text"
        onInput={(e) => {
          setSearchQuery(e.target.value);
        }}
        label="Enter a city name"
        variant="outlined"
        placeholder="Search..."
        size="small"
      />
      <IconButton type="submit" aria-label="search">
        <Icon style={{ fill: "blue" }} />
      </IconButton>
    </form>
  );

export default SearchBar;