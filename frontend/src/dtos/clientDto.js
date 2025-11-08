export class ClientDto {
  constructor(clientId, name, email, phone, preferredNotification, currentBalance) {
    this.clientId = clientId;
    this.name = name;
    this.email = email;
    this.phone = phone;
    this.preferredNotification = preferredNotification;
    this.currentBalance = currentBalance;
  }
}