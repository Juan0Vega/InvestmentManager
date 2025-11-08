const API_URL = 'https://qd0vkbuo9l.execute-api.us-east-1.amazonaws.com/api/Transactions';

export const fetchTransactionsByClientId = async (clientId) => {
  try {
    const response = await fetch(`${API_URL}/${clientId}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });
    if (!response.ok) {
      throw new Error('Error al obtener las transacciones');
    }
    return await response.json();
  } catch (error) {
    console.error('Error fetching transactions:', error);
    throw error;
  }
};

export const cancelTransaction = async (clientId, transactionId) => {
  try {
    const response = await fetch(`${API_URL}/CancelSubscription`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ clientId, transactionId }),
    });
    if (!response.ok) {
      const text = await response.text();
      throw new Error(`Cancel failed: ${response.status} ${text}`);
    }
    return await response.json().catch(() => null);
  } catch (error) {
    console.error('Error cancelling transaction:', error);
    throw error;
  }
};


export const createTransaction = async (transactionPayload) => {
  try {
    const bodyObj = {
      transactionId: transactionPayload.transactionId ?? "",
      clientId: transactionPayload.clientId,
      fundId: transactionPayload.fundId,
      fundName: transactionPayload.fundName ?? "",
      type: transactionPayload.type ?? "OPEN",
      amount: Number(transactionPayload.amount ?? 0),
     
      timestamp:
        transactionPayload.timestamp && String(transactionPayload.timestamp).trim()
          ? String(transactionPayload.timestamp)
          : new Date().toISOString(),
      balanceAfter: transactionPayload.balanceAfter ?? 0
    };

    const body = JSON.stringify(bodyObj);
    console.log('CreateTransaction payload:', body);

    const response = await fetch(`${API_URL}/CreateTransaction`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body,
    });

    const text = await response.text();
    console.log('CreateTransaction response text:', text, 'status:', response.status);

    if (!response.ok) {
      throw new Error(`Create failed: ${response.status} ${text}`);
    }

    return text ? JSON.parse(text) : null;
  } catch (error) {
    console.error('Error creating transaction:', error);
    throw error;
  }
};