import '../Components/Filter.css'
import React, { useEffect, useState } from 'react';
import Select from 'react-select';

const Filter = ({ onFilterChange }) => {
    const [category, setCategory] = useState(null);
    const [region, setRegion] = useState(null);
    const [startDate, setStartDate] = useState(null);
    const [endDate, setEndDate] = useState(null);
    const [searchTerm, setSearchTerm] = useState(null);
    const [sorting, setSorting] = useState(null);

    const handleFilterChange = () => {
        onFilterChange({ category, region, startDate, endDate, searchTerm, sorting });
    };

    return (
        <div className="filter-container">
            <div className='filter-item'>
                <label>Search Bar:</label>
                <input type='text' value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} placeholder='Search...'></input>
            </div>
            <div className="filter-item">
                <label>Date of Posting Range:</label>
                <label>Start Date:</label>
                <input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} />
                <label>End Date:</label>
                <input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} />
            </div>
            <div className="filter-item">
                <label>News by time period:</label>
                <select value={sorting} onChange={(e) => setSorting(e.target.value)}>
                    <option value="">Select</option>
                    <option value="0">Latest</option>
                    <option value="1">Most Popular</option>
                    <option value="2">Published Last Week</option>
                    <option value="3">Published Last Month</option>
                </select>
            </div>
            <div className='filter-item'>
                <label>Search by region:</label>
                <input type='text' value={region} onChange={(e) => setRegion(e.target.value)} placeholder='Type desired region...'></input>
            </div>
            <div className="filter-item">
                <label>Search by Category:</label>
                <input type='text' value={category} onChange={(e) => setCategory(e.target.value)} placeholder='Type desired news category...'></input>
            </div>
            <div className="submit-container">
                <button onClick={handleFilterChange} type="submit" className="submit">
                    Apply Filters
                </button>
            </div>
        </div>
    );
};

export default Filter;