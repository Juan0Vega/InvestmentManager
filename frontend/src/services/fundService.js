export const fetchAllFunds = async () => {
  const API_URL = 'https://qd0vkbuo9l.execute-api.us-east-1.amazonaws.com/api/Funds/GetAllFunds';
  try {
    const resp = await fetch(API_URL, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
    if (!resp.ok) throw new Error(`Error fetching funds: ${resp.status}`);
    return await resp.json();
  } catch (err) {
    console.error('fetchAllFunds error', err);
    throw err;
  }
};