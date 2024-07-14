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
      let cat =  axios.get('https://localhost:7249/news/categories')
            .then(response => setCategories(response.data))
            .catch(error => console.error('Error fetching categories', error));
            console.log(cat);
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
                <label>News Popularity:</label>
                <select value={popularity} onChange={(e) => setPopularity(e.target.value)}>
                    <option value="">Select</option>
                    <option value="1">Soonest</option>
                    <option value="2">Most Popular</option>
                </select>
            </div>
            <div className="filter-item">
                <label>News Category:</label>
                <Select
                    options={categories.map(category => ({ value: category.id, label: category.name }))}
                    value={selectedCategory}
                    onChange={(option) => setSelectedCategory(option.value)}
                />
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