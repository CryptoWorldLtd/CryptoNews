import '../Components/Filter.css'
import React, {useState , useEffect} from 'react';
import Select from 'react-select';
import axios from 'axios';

const Filter = ({ onFilterChange }) => {
    const [categories, setCategories] = useState([]);
    const [selectedCategory, setSelectedCategory] = useState(null);
    const [date, setDate] = useState('');
    const [popularity, setPopularity] = useState('');

    useEffect(() => {
        // Fetch categories from the backend
        axios.get('https://localhost:7249/news/categories')
            .then(response => setCategories(response.data))
            .catch(error => console.error('Error fetching categories', error));
    }, []);

    const handleFilterChange = () => {
        onFilterChange({ date, popularity, category: selectedCategory });
    };

    return (
        <div className="filter-container">
            <div className="filter-item">
                <label>Date of Posting:</label>
                <input type="date" value={date} onChange={(e) => setDate(e.target.value)} />
            </div>
            <div className="filter-item">
                <label>Popularity:</label>
                <select value={popularity} onChange={(e) => setPopularity(e.target.value)}>
                    <option value="">Select</option>
                    <option value="1">Soonest</option>
                    <option value="2">Most Popular</option>
                </select>
            </div>
            <div className="filter-item">
                <label>Category:</label>
                <Select
                    options={categories.map(category => ({ value: category.id, label: category.name }))}
                    value={selectedCategory}
                    onChange={(option) => setSelectedCategory(option.value)}
                />
            </div>
            <button onClick={handleFilterChange}>Apply Filters</button>
        </div>
    );
};


export default Filter;