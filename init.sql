CREATE OR REPLACE FUNCTION calculate_statistics()
RETURNS TABLE (
    sum_of_integers BIGINT,
    median_of_decimals DECIMAL
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        SUM(integer_value) as sum_of_integers,
        PERCENTILE_CONT(0.5) WITHIN GROUP(ORDER BY decimal_value) as median_of_decimals
    FROM data_rows;
END;
$$ LANGUAGE plpgsql;