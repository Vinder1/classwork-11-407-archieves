const getHolidayInfo = document.getElementById("getHoliday").addEventListener("click", async () => {
    let info = await fetch("/api/holiday");
    let holidayTextBlock = document.getElementById("holiday");

    if (holidayTextBlock == null) {
        holidayTextBlock = document.createElement('p');
        holidayTextBlock.id = 'holiday';
        holidayTextBlock.classList.add('holiday-text');
        document.body.children[1].children[0].appendChild(holidayTextBlock);
    } 

    let json = await info.json();
    let holiday = json.holidays[0];
    let holidayDate = new Date(holiday.date);
    let monthName = new Date(2024, holidayDate.getMonth() - 1).toLocaleDateString('ru-RU', { month: 'long' });

    holidayTextBlock.innerHTML = `${holiday.name} (${holidayDate.getDay()}, ${monthName})`;
});