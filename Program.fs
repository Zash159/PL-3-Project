open System
open System.Text.RegularExpressions

// Define the Contact type
type Contact = {
    Name: string
    PhoneNumber: string
    Email: string
}

// Initialize the contact list
let mutable contacts: Contact list = []

// Validate phone number (must be 11 digits)
let isValidPhoneNumber (phone: string) =
    let regex = @"^\d{11}$"  // Must be exactly 11 digits
    Regex.IsMatch(phone, regex)

// Validate email (basic validation)
let isValidEmail (email: string) =
    let regex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"  // Simple email pattern
    Regex.IsMatch(email, regex)

// Validate name (at least 4 characters)
let isValidName (name: string) =
    name.Length >= 4

// Add a contact
let addContact (contact: Contact) =
    contacts <- contact :: contacts
    printfn "Contact added successfully!"

// Search for contacts
let searchContacts (query: string) =
    let queryLower = query.ToLower()
    let results = 
        contacts |> List.filter (fun c -> 
            c.Name.ToLower().Contains(queryLower) || 
            c.PhoneNumber.ToLower().Contains(queryLower))
    results

// View all contacts
let viewAllContacts () =
    if List.isEmpty contacts then
        printfn "No contacts available."
    else
        contacts |> List.iter (fun c -> 
            printfn "Name: %s | Phone: %s | Email: %s" c.Name c.PhoneNumber c.Email)

// Edit a contact
let editContact (oldContact: Contact) (newContact: Contact) =
    contacts <- contacts |> List.map (fun c -> 
        if c = oldContact then newContact else c)
    printfn "Contact updated successfully!"

// Delete a contact
let deleteContact (contact: Contact) =
    contacts <- contacts |> List.filter (fun c -> c <> contact)
    printfn "Contact deleted successfully!"

// Display the main menu
let printMenu () =
    printfn "\n--- Contact Management System ---"
    printfn "1. Add Contact"
    printfn "2. Search Contacts"
    printfn "3. View All Contacts"
    printfn "4. Delete Contact"
    printfn "5. Exit"

// Handle adding a contact with validation
let handleAddContact () =
    // Validate name
    let nameValid = 
        printfn "Enter Name (at least 4 characters):"
        let name = Console.ReadLine()
        if isValidName name then Some name else 
            printfn "Name must be at least 4 characters long."; None

    match nameValid with
    | Some name ->
        // Validate phone number
        printfn "Enter Phone Number (11 digits only):"
        let phone = Console.ReadLine()
        if not (isValidPhoneNumber phone) then 
            printfn "Phone number must be exactly 11 digits."
            None
        else
            // Validate email
            printfn "Enter Email (optional):"
            let email = Console.ReadLine()
            if email <> "" && not (isValidEmail email) then
                printfn "Invalid email format."
                None
            else
                Some (name, phone, email)
    | None -> None

// Handle searching for contacts
let handleSearchContacts () =
    printfn "Enter search query (Name or Phone):"
    let query = Console.ReadLine()
    let results = searchContacts query

    if List.isEmpty results then
        printfn "No results found."
    else
        printfn "Search Results:"
        results |> List.iter (fun c -> 
            printfn "Name: %s | Phone: %s | Email: %s" c.Name c.PhoneNumber c.Email)

// Handle viewing all contacts
let handleViewAllContacts () =
    viewAllContacts ()

// Handle deleting a contact
let handleDeleteContact () =
    printfn "Enter the Name of the contact to delete:"
    let nameToDelete = Console.ReadLine()
    let contactToDelete = contacts |> List.tryFind (fun c -> c.Name.ToLower() = nameToDelete.ToLower())

    match contactToDelete with
    | Some c -> deleteContact c
    | None -> printfn "Contact not found."

// Main application loop
[<EntryPoint>]
let main _ =
    let mutable running = true

    while running do
        printMenu ()
        printfn "Select an option (1-5):"
        let option = Console.ReadLine()

        match option with
        | "1" ->
            match handleAddContact () with
            | Some (name, phone, email) -> 
                let contact = { Name = name; PhoneNumber = phone; Email = email }
                addContact contact
            | None -> printfn "Failed to add contact due to invalid data."
        | "2" -> handleSearchContacts ()
        | "3" -> handleViewAllContacts ()
        | "4" -> handleDeleteContact ()
        | "5" -> running <- false
        | _ -> printfn "Invalid option, please try again."

    printfn "Goodbye!"
    0