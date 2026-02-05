CREATE PROCEDURE holiday.delete_holiday(holiday_id INT) AS $$
    DELETE FROM holiday.Holiday
    WHERE holidayId = holiday_id
$$ LANGUAGE SQL;

CREATE FUNCTION holiday.check_balance(user_login TEXT)
RETURNS BOOLEAN AS $$
DECLARE
    current_coins INT;
BEGIN
    SELECT coins INTO current_coins
    FROM holiday.user
    WHERE login = user_login;

    RETURN current_coins IS NOT NULL AND current_coins > 5;
END;
$$ LANGUAGE plpgsql